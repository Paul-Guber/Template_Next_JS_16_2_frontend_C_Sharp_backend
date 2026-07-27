'use client'
import style from './view.module.scss'
import Tittle from '../UI/Tittle/Tittle'
import Button from '../UI/Buttons/Button'
import MyForm from '../UI/Forms/MyForm'
import MyFormInput from '../UI/FormInput/MyFormInput'
import {
	emailOptions,
	phoneNumberOptions,
	userNameOptions,
} from '@/utils/optionsForInputs'
import Search from '../Filters/Search'
import React from 'react'
import UpdateSvg from '@svg/update-arrows.svg'
import DeleteSvg from '@svg/delete.svg'
import ToolTip from '../UI/Tooltips/ToolTip'
const ViewData = ({ data }: { data: IEmployee[] }) => {
	return (
		<>
			<Tittle tittleText='Список всех сотрудников' />
			<div className={style['container']}>
				<Search placeholder='Поиск...' />
				{data.length > 0 ? (
					<>
						<div className={style['flex-table']}>
							<div className={style['flex-table-header']}>
								<div className={style['flex-cell']}>Имя сотрудника</div>
								<div className={style['flex-cell']}>Email сотрудника</div>
								<div className={style['flex-cell']}>Телефон</div>
								<div className={style['flex-cell']}> </div>
							</div>
							{data.map((item) => (
								<React.Fragment key={item.id.toString()}>
									<div className={style['flex-table__inner']}>
										<MyForm<IEmployeeDto>
											fetchPath={`/employee/update/${item.id}`}
											styleForm={style['flex-table-row']}
											options={{
												method: 'PUT',
												headers: {
													'Content-Type': 'application/json',
												},
											}}>
											<div className={style['flex-cell']}>
												<MyFormInput<IEmployeeDto>
													inputName='name'
													labelInput=''
													placeholder='Введите имя'
													options={{ ...userNameOptions, value: item.name }}
												/>
											</div>
											<div className={style['flex-cell']}>
												<MyFormInput<IEmployeeDto>
													inputName='email'
													labelInput=''
													placeholder='Введите Email'
													options={{ ...emailOptions, value: item.email }}
												/>
											</div>
											<div className={style['flex-cell']}>
												<MyFormInput<IEmployeeDto>
													inputName='phone'
													labelInput=''
													placeholder='+79991113367'
													options={{
														...phoneNumberOptions,
														value: item.phone,
													}}
												/>
											</div>
											<div className={style['flex-cell']}>
												<button
													className={`${style.btn} ${style['btn--left']}`}
													type='submit'>
													<ToolTip tooltipText='Обновить' position='right'>
														<UpdateSvg className={style.svg} />
													</ToolTip>
												</button>
											</div>
										</MyForm>
										<MyForm<IEmployeeDto>
											fetchPath={`/employee/delete/${item.id}`}
											styleForm={`${style.btn} ${style['btn--right']}`}
											options={{
												method: 'DELETE',
												headers: {
													'Content-Type': 'application/json',
												},
											}}>
											<button type='submit'>
												<ToolTip tooltipText='Удалить' position='left'>
													<DeleteSvg className={style.svg} />
												</ToolTip>
											</button>
										</MyForm>
									</div>
								</React.Fragment>
							))}
						</div>
						<div>
							<MyForm<IEmployeeDto>
								fetchPath={`/employee/fullDelete`}
								options={{
									method: 'DELETE',
									headers: {
										'Content-Type': 'application/json',
									},
								}}>
								<Button
									props={{
										type: 'submit',
										style: {
											width: 'auto',
											maxWidth: '300px',
											padding: '0 10px',
										},
									}}>
									Удалить всех сотрудников
								</Button>
							</MyForm>
						</div>
					</>
				) : (
					<>
						<Tittle tittleText='Сотрудник не найден' />
					</>
				)}
			</div>
		</>
	)
}

export default ViewData
