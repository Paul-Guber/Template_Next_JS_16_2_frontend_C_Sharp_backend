'use client'
import style from './pagination.module.scss'
import { pageSize } from '@/utils/configPagination'
import Link from 'next/link'
import { usePathname, useRouter, useSearchParams } from 'next/navigation'
import { ChangeEvent, useEffect, useState } from 'react'
import Arrow_Left_Svg from '@svg/arrow-left.svg'
import Arrow_Right_Svg from '@svg/arrow-right.svg'
import { useDebouncedCallback } from 'use-debounce'
export default function Pagination({ totalCount }: { totalCount: number }) {
	const totalPageCount: number = Math.ceil(totalCount / pageSize)

	const router = useRouter()
	const pathname = usePathname()
	const searchParams = useSearchParams()
	// Текущая страница
	const currentPage = Number(searchParams.get('page')) || 1
	const [valueInput, setValueInput] = useState('0')

	useEffect(() => {
		setValueInput(currentPage.toString())
		if (currentPage <= 0) {
			const pageParam = new URLSearchParams(searchParams)
			pageParam.set('page', '1')
			router.replace(`${pathname}?${pageParam.toString()}`)
		}

		if (currentPage >= totalPageCount) {
			const pageParam = new URLSearchParams(searchParams)
			pageParam.set('page', totalPageCount.toString())
			router.replace(`${pathname}?${pageParam.toString()}`)
		}
	}, [currentPage, totalPageCount])

	const setPageInput = useDebouncedCallback(
		(e: ChangeEvent<HTMLInputElement, HTMLInputElement>) => {
			const value = e.target.value.replace(/\D+/g, '')
			const parse: number = value.trim() !== '' ? parseInt(value) : 1
			const pageNumber: number =
				parse <= 0 ? 1 : parse >= totalPageCount ? totalPageCount : parse
			setValueInput(pageNumber.toString())
			const pageParam = new URLSearchParams(searchParams)
			pageParam.set('page', pageNumber.toString())
			router.replace(`${pathname}?${pageParam.toString()}`)
			e.target.value = pageNumber.toString()
		},
		1000,
	)
	return (
		<>
			<div className={style.flex}>
				<div className={style.pagination}>
					<Link
						className={`${style.link} ${style['link--left']}`}
						rel='prev'
						href={`?page=${+currentPage > 1 ? +currentPage - 1 : +currentPage}`}>
						<Arrow_Left_Svg className={style.svg} />
					</Link>
					<p>
						Страница
						<input
							className={style.input}
							value={valueInput}
							onChange={(e) => {
								setValueInput(e.target.value)
								setPageInput(e)
							}}
						/>
						из {totalPageCount}
					</p>
					<Link
						rel='next'
						className={`${style.link} ${style['link--right']}`}
						href={`?page=${+currentPage < totalPageCount ? +currentPage + 1 : +currentPage}`}>
						<Arrow_Right_Svg className={style.svg} />
					</Link>
				</div>
			</div>
		</>
	)
}
