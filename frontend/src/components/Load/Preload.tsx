import style from './preload.module.scss'
const Preload = () => {
	return (
		<>
			<div className={style['loading']}>
				<div className={style['loading__text']}></div>
			</div>
		</>
	)
}

export default Preload
