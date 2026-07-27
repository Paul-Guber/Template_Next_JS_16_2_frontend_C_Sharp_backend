'use client'

import NotificationMessage from '@/components/Notification/NotificationMessage'
import fetchClientApi from '@/utils/clientActions'
import { isResponseErrors, isSuccessResponse } from '@/utils/typeGuards'
import { useRouter } from 'next/navigation'
import { ReactNode, useState } from 'react'

import {
	useForm,
	SubmitHandler,
	FormProvider,
	DefaultValues,
	Path,
} from 'react-hook-form'

type Forms<T extends Record<string, any>> = {
	fetchPath: string
	defaultValues?: T extends object ? DefaultValues<T> : never
	options?: RequestInit
	children?: ReactNode
	styleForm?: string
}
export default function MyForm<T extends Record<string, string>>({
	defaultValues,
	children,
	options,
	fetchPath,

	styleForm,
}: Forms<T>) {
	const router = useRouter()
	const [responseData, setResponseData] = useState<unknown>(undefined)
	const methods = useForm<T>({
		defaultValues: defaultValues as DefaultValues<T> | undefined,
	})

	const { handleSubmit, setError, reset } = methods
	const submitForm: SubmitHandler<T> = async (data: T) => {
		const response = await fetchClientApi<T>(fetchPath, {
			method: options?.method ? options.method : 'POST',
			body: JSON.stringify(data),
			...options,
		})

		if (isResponseErrors(response)) {
			response.map((item) => {
				item.errorMessage &&
					item.propertyName &&
					setError(item.propertyName.toLowerCase() as Path<T>, {
						message: item.errorMessage,
					})
			})
		}

		if (isSuccessResponse(response)) {
			setResponseData(response)

			// router.refresh()
		}
		options?.method === 'POST' ? reset() : ''
	}

	return (
		<>
			<FormProvider {...methods}>
				<form
					onSubmit={handleSubmit(submitForm)}
					className={styleForm ? styleForm : ''}>
					{children}
				</form>
			</FormProvider>
			<NotificationMessage<T>
				data={responseData}
				callBack={(isSuccess: boolean) => {
					router.refresh()
				}}
			/>
		</>
	)
}
