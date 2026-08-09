export function isResponseErrors(response: any): response is IResponseErrors[] {
	return (
		Array.isArray(response) &&
		response.every((item) => {
			return (
				typeof item === 'object' &&
				item !== null &&
				(typeof item.propertyName === 'string' ||
					typeof item.propertyName === 'undefined' ||
					typeof item.propertyName === null) &&
				(typeof item.errorMessage === 'string' ||
					typeof item.errorMessage === 'undefined' ||
					typeof item.errorMessage === null) &&
				(typeof item.message === 'string' ||
					typeof item.message === 'undefined' ||
					typeof item.message === null)
			)
		})
	)
}

export function isResponseError(obj: any): obj is IResponseErrors {
	if (typeof obj !== 'object' || obj === null) return false
	const response = obj as Record<string, unknown>

	return (
		(typeof response.propertyName === 'string' ||
			typeof response.propertyName === 'undefined' ||
			typeof response.propertyName === null) &&
		(typeof response.errorMessage === 'string' ||
			typeof response.errorMessage === 'undefined' ||
			typeof response.errorMessage === null) &&
		(typeof response.message === 'string' ||
			typeof response.message === 'undefined' ||
			typeof response.message === null)
	)
}

export function isSuccessResponse<T>(obj: any): obj is IResponse<T> {
	if (typeof obj !== 'object' || obj === null) return false
	const response = obj as Record<string, unknown>
	return (
		(typeof response.message === 'string' ||
			typeof response.message === null ||
			typeof response.message === 'undefined') &&
		(typeof response.totalCount === 'number' ||
			typeof response.message === null ||
			typeof response.message === 'undefined') &&
		(typeof response.data === 'object' || typeof response.data === 'undefined' || typeof response.data === null)
	)
}
