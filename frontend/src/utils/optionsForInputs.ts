const minUserName = 3
const maxUserName = 50
const minEmailValue = 3
const maxEmailValue = 50

export const emailOptions = {
	required: { message: 'Поле обязательно для заполнения!', value: true },
	pattern: {
		value:
			/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
		message: 'Неверный адрес электронной почты!',
	},
	minLength: {
		value: minEmailValue,
		message: `В поле должно быть не менее ${minEmailValue} символов.`,
	},
	maxLength: {
		value: maxEmailValue,
		message: `В поле должно быть не более ${maxEmailValue} символов`,
	},
}

export const userNameOptions = {
	required: { message: 'Поле обязательно для заполнения!', value: true },
	minLength: {
		value: minUserName,
		message: `Поле должно содержать минимум ${minUserName} символа!`,
	},
	maxLength: {
		value: maxUserName,
		message: `Поле не должно превышать ${maxUserName} символов!`,
	},
	pattern: {
		value: /^[0-9a-zA-Zа-яёА-ЯЁ\s_-]+$/i,
		message: 'Разрешены только буквы.',
	},
}
export const phoneNumberOptions = {
	required: { message: 'Поле обязательно для заполнения!', value: true },
	pattern: {
		value: /^((\+7|7|8)+([0-9]){10})$/,
		message: 'Неверный формат номера телефона!',
	},
}
