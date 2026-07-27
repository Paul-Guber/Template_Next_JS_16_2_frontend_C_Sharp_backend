'use client'
import { ReactNode } from 'react'
import style from './main.module.scss'
import {
	emailOptions,
	phoneNumberOptions,
	userNameOptions,
} from '@/utils/optionsForInputs'
import MyForm from '../UI/Forms/MyForm'
import MyFormInput from '../UI/FormInput/MyFormInput'
import Button from '../UI/Buttons/Button'

const MainForm = ({ children }: { children?: ReactNode }) => {
	return (
		<>
			{/* Right Panel */}
			<div className={`${style['right-panel']}`}>{children}</div>

			{/* Left Panel */}
			<div className={`${style['left-panel']}`}>
				<MyForm<IEmployeeDto>
					defaultValues={undefined}
					styleForm={style.form}
					fetchPath={`/employee/add`}
					options={{
						method: 'POST',
						headers: {
							'Content-Type': 'application/json',
						},
					}}>
					<h1 className={style['text-h1']}> Добавить сотрудника </h1>
					<span className={style['text-span']}>
						для добавления сотрудника заполните ниже поля
					</span>
					<MyFormInput<IEmployeeDto>
						inputName='name'
						labelInput='Имя'
						isViewStar
						placeholder='Введите имя'
						options={userNameOptions}
					/>
					<MyFormInput<IEmployeeDto>
						inputName='email'
						isViewStar
						labelInput='Email'
						placeholder='Введите Email'
						options={emailOptions}
					/>
					<MyFormInput<IEmployeeDto>
						inputName='phone'
						isViewStar
						labelInput='Телефон'
						placeholder='+79993332211'
						options={phoneNumberOptions}
					/>
					<div className={style.flex}>
						<Button
							textBtn='Добавить'
							styleBtn='header__btn'
							props={{
								type: 'submit',
							}}
						/>
					</div>
				</MyForm>
			</div>
		</>
	)
}

export default MainForm
