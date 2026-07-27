'use server'

import { apiServer } from './serverName'
export const fetchApi = async <T = void, T2 = string>(
	fullPathUrl: string,
	options: RequestInit,
) => {
	const res = await fetch(`${apiServer}/api${fullPathUrl}`, {
		...options,
		credentials: 'include',
	})
	if (res.ok) {
		const response: T = await res.json()

		return response
	}
	return undefined
}
