import type { NextConfig } from 'next'

const nextConfig: NextConfig = {
	basePath: '',
	reactStrictMode: true,
	trailingSlash: false,
	productionBrowserSourceMaps: true,
	async redirects() {
		return [
			{
				source: '/',
				destination: '/employee', // путь к странице из папки
				permanent: false, // или true для постоянного редиректа
			},
		]
	},
	experimental: {
		serverActions: {
			bodySizeLimit: '50mb',
		},
	},
	turbopack: {
		rules: {
			'*.svg': {
				condition: {
					all: [
						{ not: { path: '*.url.svg' } },

						// Optional: skip node_modules for possible performance
						{ not: 'foreign' },
					],
				},
				loaders: ['@svgr/webpack'],
				as: '*.js',
			},
		},
	},
}
module.exports = nextConfig
