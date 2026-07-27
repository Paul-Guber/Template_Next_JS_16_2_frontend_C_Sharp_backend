'use server'
import MainLayout from '@/components/Main/MainLayout'

export default async function page({ searchParams }: searchParams) {
	const params = await searchParams
	const query = params?.search || ''
	const currentPage: number = Number(params?.page) || 1

	return (
		<main>
			<MainLayout currentPage={currentPage} searchQuery={query} />
		</main>
	)
}
