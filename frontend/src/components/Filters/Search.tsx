'use client'
import { useDebouncedCallback } from 'use-debounce'
import style from './search.module.scss'
import { usePathname, useRouter, useSearchParams } from 'next/navigation'
type TypeSearch = {
	placeholder: string
}
export default function Search({ placeholder }: TypeSearch) {
	const searchParams = useSearchParams()
	const pathname = usePathname()
	const { replace } = useRouter()
	const handleSearch = useDebouncedCallback((term: string) => {
		const params = new URLSearchParams(searchParams)

		if (term && term.trim() !== '') {
			params.set('search', term)
		} else {
			params.delete('search')
		}
		replace(`${pathname}?${params.toString()}`)
	}, 1500)
	return (
		<>
			<input
				className={style.input}
				type='text'
				placeholder={placeholder}
				onChange={(e) => {
					handleSearch(e.target.value)
				}}
			/>
		</>
	)
}
