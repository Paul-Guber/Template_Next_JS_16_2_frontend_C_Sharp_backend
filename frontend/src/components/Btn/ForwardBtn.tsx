'use client'
import style from './forwardBtn.module.scss'
import Link from 'next/link'
import Button from '../UI/Buttons/Button'
import { ReactNode } from 'react'

const ForwardBtn = ({
	href,
	textLink,
	children,
}: {
	href: string
	textLink?: string
	children?: ReactNode
}) => {
	return (
		<>
			<div className={style.flex}>
				<Button isGray>
					<Link href={href} className={style.link}>
						{children ? children : textLink ? textLink : 'Назад'}
					</Link>
				</Button>
			</div>
		</>
	)
}

export default ForwardBtn
