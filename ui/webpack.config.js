const path = require('path');
const CSSExt = require('mini-css-extract-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const { VueLoaderPlugin } = require('vue-loader');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const { WebpackManifestPlugin} = require("webpack-manifest-plugin");
const WebpackBar = require('webpackbar');

let source = path.resolve(__dirname, "src");
let target = path.resolve(__dirname, "..", "server", "wwwroot");

module.exports = (env, argv) => {
	let dev = argv.mode == 'development';
	return {
		mode: dev ? "development" : "production",
		entry: [ 
			path.resolve(source, "js", "index.ts"),
			path.resolve(source, "style", "index.scss") 
		],
		output: {
			path: target,
			filename: dev ? "[name].js" : "[name].[contenthash].js",
			clean: true,
			publicPath: '/'
		},
		stats: 'errors-warnings',
		module: {
			rules: [
				{
					test: /\.css$/i,
					use: [CSSExt.loader, 'css-loader']
				},
				{
					test: /\.s[ac]ss$/i,
					use: [CSSExt.loader, 'css-loader', 'sass-loader']
				},
				{
					test: /\.ts$/i,
					loader: 'ts-loader',
					options: {
						appendTsSuffixTo: [/\.vue$/],
					},
					exclude: /node_modules/
				},
				{
					test: /\.vue$/i,
					use: ['vue-loader']
				},
				{
					test: /\.(otf)|(woff)|(ttf)|(png)|(jpg)|(jpeg)|(mp4)|(mp3)|(webm)$/i,
					type: 'asset/resource'
				}
			]
		},
		resolve: {
			extensions: [ '.ts', '.js', '.vue' ],
			alias: {
				'@': path.resolve(source, "js"),
				'#': path.resolve(source, 'style'),
				'£': path.resolve(source, 'resources')
			}
		},
		plugins: [
			new CSSExt({
				filename: dev ? 'style.css' : "style.[contenthash].css"
			}),
			new HtmlWebpackPlugin({
				template: path.resolve(source, "index.template.html"),
				filename: path.resolve(target, "index.html"),
				inject: false,
				minify: dev ? false : 'auto'
			}),
			new VueLoaderPlugin(),
			/*new CopyWebpackPlugin({
				patterns: [
					{ from: 'src/resources', to: "" }
				]
			})*/
			new WebpackManifestPlugin(),
			new WebpackBar()
		]
	}
};