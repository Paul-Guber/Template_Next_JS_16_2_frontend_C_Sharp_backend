'use client'

import { ReactNode } from 'react'
import style from './tooltip.module.scss'
type TypePosition = 'left' | 'top' | 'right' | 'bottom'
type TypeToolTip = {
	children?: ReactNode
	tooltipText?: string
	position?: TypePosition
}
const ToolTip = ({
	children,
	position = 'top',
	tooltipText = 'Подсказка по умолчанию',
}: TypeToolTip) => {
	return (
		<>
			<div className={style['tooltip-trigger']}>
				{children}
				<div className={`${style.tooltip} ${style[`tooltip-${position}`]}`}>
					{tooltipText}
				</div>
			</div>
		</>
	)
}

export default ToolTip
