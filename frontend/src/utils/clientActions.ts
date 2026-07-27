'use client'

import { apiServer } from './serverName'
import { isResponseErrors, isSuccessResponse } from './typeGuards'

const fetchClientApi = async <T = void>(
	fullPathUrl: string,
	options: RequestInit,
): Promise<IResponse<T> | IResponseErrors[] | undefined> => {
	try {
		const res = await fetch(`${apiServer}/api${fullPathUrl}`, {
			...options,
			credentials: 'include',
		})

		const response: unknown = await res.json()

		if (isResponseErrors(response)) {
			return response
		} else if (isSuccessResponse<T>(response)) {
			return response
		} else {
			return undefined
		}
	} catch (error) {
		console.log('Catch Errors')

		if (error instanceof Error) {
			console.log(' error message = ')
			console.log(error.message)
		} else if (error instanceof Object) {
			console.log('error is Object = ', error)
		} else {
			console.log('Неизвестная ошибка:', error)
		}
	}
}

export default fetchClientApi
