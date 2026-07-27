import { configureStore } from '@reduxjs/toolkit'
import managerResponse from './slices/response-slice'

export const makeStore = () => {
	return configureStore({
		reducer: {
			managerResponseInfo: managerResponse,
		},
		devTools: false,
	})
}
export type AppStore = ReturnType<typeof makeStore>
export type RootState = ReturnType<AppStore['getState']>
export type AppDispatch = AppStore['dispatch']
