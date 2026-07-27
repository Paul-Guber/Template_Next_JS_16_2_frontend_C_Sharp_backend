import { apiServer } from '@/utils/serverName'
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

export const BaseApi = createApi({
	reducerPath: 'baseApi',
	baseQuery: fetchBaseQuery({
		baseUrl: apiServer,
	}),
	tagTypes: ['All', 'AuthUser', 'User'],
	endpoints: (_build) => ({}),
})
