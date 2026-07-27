import { ButtonHTMLAttributes, MouseEventHandler, ReactNode } from 'react'
import style from './button.module.scss'
type TypeBtn = {
	textBtn?: string
	isGray?: boolean
	styleBtn?: string
	onClick?: MouseEventHandler<HTMLButtonElement>
	props?: ButtonHTMLAttributes<HTMLButtonElement>
	children?: ReactNode
}
const Button = ({
	textBtn,
	onClick,
	props,
	styleBtn,
	isGray,
	children,
}: TypeBtn) => {
	return (
		<button
			className={`${isGray ? style['btn--gray'] : style['btn']} ${styleBtn ? styleBtn : ''}`}
			onClick={onClick}
			{...props}>
			{children ? children : textBtn ? textBtn : 'Button'}
		</button>
	)
}

export default Button
