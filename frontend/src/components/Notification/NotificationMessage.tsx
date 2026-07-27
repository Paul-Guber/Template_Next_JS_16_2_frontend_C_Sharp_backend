'use client'
import { isResponseError, isSuccessResponse } from '@/utils/typeGuards'
import style from './message.module.scss'
import { useEffect, useState } from 'react'

const NotificationMessage = <T,>({
	data,
	callBack,
}: {
	data: unknown
	callBack?: (e: boolean) => void
}) => {
	const [message, setMessage] = useState<string | undefined>(undefined)
	const [isCallBack, setIsCallBack] = useState<boolean>(false)

	useEffect(() => {
		if (isResponseError(data)) {
			if (data.message && data.message.trim() != '') {
				setMessage(data.message)
				setIsCallBack(true)
			}
			if (data.errorMessage && data.errorMessage.trim() != '') {
				setMessage(data.errorMessage)
				setIsCallBack(false)
				callBack && callBack(false)
			}
		} else if (isSuccessResponse<T>(data)) {
			data.message && data.message.trim() !== '' && setMessage(data.message)
		}
	}, [data])
	useEffect(() => {
		// Сообщение исчезнет через 6 секунд
		const timeoutId = setTimeout(() => {
			if (message) {
				setMessage(undefined)
				callBack && callBack(isCallBack)
			}
		}, 6000)

		return () => clearTimeout(timeoutId)
	}, [message])

	return (
		<>
			{message && message.trim() != '' && (
				<>
					<div
						className={`${
							message
								? `${style.notification} ${style['notification--active']}`
								: style.notification
						} ${style['notification__inner']}`}>
						<p className={style['notification__text']}>{message}</p>
						<div className={`${style['notification__progress']}`}></div>
					</div>
				</>
			)}
		</>
	)
}

export default NotificationMessage
