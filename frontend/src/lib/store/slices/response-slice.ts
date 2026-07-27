import { PayloadAction, createSlice } from '@reduxjs/toolkit'

const initialState: IResponse = {
	data: undefined,
	message: '',
	totalCount: 0,
}
export const managerResponse = createSlice({
	name: 'User Account',
	initialState,
	reducers: {
		setManagerAccount: (state, action: PayloadAction<IResponse>) => {
			state.data = action.payload.data
			state.message = action.payload.message
			state.totalCount = action.payload.totalCount
		},
	},
})
export const { setManagerAccount } = managerResponse.actions
export default managerResponse.reducer
