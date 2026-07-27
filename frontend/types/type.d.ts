import { Guid } from 'js-guid'

export {}

declare global {
	interface IResponse<TData = void> {
		data?: TData
		totalCount: number
		message?: string
	}
	interface IEmployeeDto {
		[key: string]: string
		name: string
		email: string
		phone: string
	}
	interface IEmployee extends IEmployeeDto {
		id: Guid
	}
	interface IResponseErrors {
		propertyName?: string | null
		errorMessage?: string | null
		message?: string | null
	}
	type params = { params: Promise<{ employeeId: string }> }
	type searchParams = {
		searchParams?: Promise<{ search?: string; page?: string }>
	}
}
