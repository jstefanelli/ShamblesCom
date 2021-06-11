<template>
	<div class="main">
		<div class="title-screen">
			<div class="background">

			</div>
			<spa-link target="test" class="foreground center-container">
				<div class="title center-item">
					<p class="small">THE</p>
					<p class="large">SHAMBLES</p>
					<p class="justified">CHAMPIONSHIP</p>
				</div>
			</spa-link>
		</div>
	</div>
</template>

<script lang="ts">
import Vue from 'vue';
import SpaLink from '@/SPA/SpaLink.vue';

export default Vue.extend({
	components: {
		"spa-link": SpaLink
	}
})
</script>

<style lang="scss">

.title-screen {
	width: 100vw;
	height: 100vh;
	position: relative;
	overflow: hidden;

	> .background {
		width: 100%;
		height: 100%;
		position: absolute;
	}

	> .foreground {
		width: 100%;
		height: 100%;
		position: absolute;
		

		> .title {
			font-style: italic;

			> p {
				margin: 0;
				padding: 0;
				width: 100%;
			}

			> .small {
				font-size: 30pt;
				padding-left: 20pt;
				line-height: 30pt;
				font-weight: bold;
			}

			> .large {
				font-style: italic;
				font-weight: bolder;
				font-size: 90pt;
				padding-left: 5pt;
				line-height: 75pt;
			}

			> .justified {
				width: calc(100% - 20pt);
				font-size: 35pt;
				line-height: 35pt;
				font-weight: bold;
				text-align: justify;
				text-justify: distribute;
				padding-right: 100px;

				&:after {
					content: '';
					display: inline-block;
					width: 100%;
				}
			}
		}
	}
}

</style>