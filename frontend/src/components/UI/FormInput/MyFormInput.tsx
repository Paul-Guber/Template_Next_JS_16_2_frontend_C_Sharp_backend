'use client'

import { Path, RegisterOptions, useFormContext } from 'react-hook-form'
import style from './form.module.scss'
import { InputHTMLAttributes, ReactNode } from 'react'

type TypeInput<T extends Record<string, any>> = {
	inputName: Path<T>
	labelInput: string
	isRequired?: boolean
	isTextErrors?: boolean
	placeholder?: string
	inputAttributes?: InputHTMLAttributes<HTMLInputElement>
	options?: RegisterOptions<Record<string, unknown> | T, Path<T>>
	errorMessage?: string
	isViewStar?: boolean
}
export default function MyFormInput<T extends Record<string, string>>(
	data: TypeInput<T>,
) {
	const {
		inputName,
		labelInput,
		inputAttributes,
		options,
		isRequired = true,
		errorMessage,
		placeholder,
		isTextErrors,
		isViewStar,
	} = data
	const {
		register,
		formState: { errors },
	} = useFormContext<Record<string, unknown> | T, any, T>()

	return (
		<>
			<div
				className={
					labelInput && labelInput.trim() !== '' ? style['input__inner'] : '	'
				}>
				<div className={`${style['input__title']}`}>
					{labelInput}
					{isRequired && isViewStar && (
						<span className={style.required}>*</span>
					)}
				</div>
				<input
					type='text'
					className={`${
						errors?.[inputName]?.message || errorMessage
							? `${style.input} ${style.error}`
							: `${style.input}`
					}`}
					placeholder={placeholder}
					{...register(inputName, options)}
					{...inputAttributes}
				/>
				{isTextErrors && (
					<span className={`${style['input__errors']} ${style.label}`}>
						{errors?.[inputName]?.message as ReactNode}
					</span>
				)}
			</div>
		</>
	)
}
