'use server'
import { fetchApi } from '@/utils/actions'
import style from './main.module.scss'
import MainForm from './MainForm'
import ViewData from './ViewData'
import Pagination from '../Filters/Pagination'
import { pageSize } from '@/utils/configPagination'
type TypeSearch = {
	currentPage: number
	searchQuery: string
}
export default async function MainLayout({
	searchQuery,
	currentPage,
}: TypeSearch) {
	const params = new URLSearchParams({
		searchQuery: searchQuery,
		page: currentPage.toString(),
		limit: pageSize.toString(),
	})

	const result = await fetchApi<IResponse<IEmployee[]>>(
		`/employee/getAll?${params}`,
		{
			method: 'GET',
			headers: {
				'Content-Type': 'application/json',
			},
		},
	)

	const getAllEmployee: IEmployee[] = result?.data ? result.data : []
	return (
		<>
			<div className={style.body}>
				<div>
					<MainForm>
						<ViewData data={getAllEmployee} />
						{getAllEmployee.length > 0 && (
							<Pagination totalCount={result?.totalCount ?? 1} />
						)}
					</MainForm>
				</div>
			</div>
		</>
	)
}
