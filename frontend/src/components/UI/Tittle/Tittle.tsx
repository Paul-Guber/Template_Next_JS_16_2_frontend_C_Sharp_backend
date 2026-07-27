import style from './tittle.module.scss'

type TypeText = {
	tittleText: string
	subtitleText?: string
	styleModuleText?: string
	styleContent?: string
}
const Tittle = ({
	tittleText: text = 'Tittle',
	subtitleText,
	styleModuleText,
	styleContent,
}: TypeText) => {
	return (
		<div className={`${style.content} ${styleContent ? styleContent : ''}`}>
			<h2
				className={`${style['h2']} ${styleModuleText ? styleModuleText : ''}`}>
				{text}
			</h2>
			<p className={style.paragraph}> {subtitleText} </p>
		</div>
	)
}

export default Tittle
