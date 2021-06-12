<template>
	<div class="main">
		<header :class="titleClass()">
			<div class="foreground flex flex-horizontal align-center">
				<div :class="'flex-grow'" />
				<spa-link target="/homepage" class="title">
					<p class="small">THE</p>
					<p class="large">SHAMBLES</p>
					<p class="justified">CHAMPIONSHIP</p>
				</spa-link>
				<div class="flex-grow" />
			</div>
		</header>
		<main :class="contentPadding()">
			<div>
			This is some content! LMAO
			</div>
		</main>
	</div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import SpaLink from '@/SPA/SpaLink.vue';

@Component({
	components: {
		SpaLink
	}
})
export default class IndexComponent extends Vue {
	private largeHeader: boolean = true;

	private titleClass(): string {
		return "title-screen " + (this.largeHeader ? '' : 'small');
	}

	private contentPadding(): string {
		return "main" + this.largeHeader ? "padding-full" : "padding-small"
	}

	public mounted(){
		window.addEventListener("scroll", this.onScroll);
	}

	public beforeDestroy(){
		window.removeEventListener("scroll", this.onScroll);
	}

	public onScroll(ev: Event) {
		this.largeHeader = document.documentElement.scrollTop < (this.largeHeader ? window.innerHeight / 2 : 90);
	}
}

</script>

<style lang="scss">

$small-header-size: 90px;

.title-screen {
	position: fixed;
	width: 100%;
	height: 100vh;
	max-height: 100vh;
	overflow: hidden;
	transition-duration: .5s;

	* {
		transition-duration: .5s;
	}

	&.small {
		max-height: $small-header-size;

		.foreground {
			position: unset;

			> .title {
				padding: 15px;

				> .small {
					font-size: 10pt;
					padding-left: 7.5pt;
					line-height: 10pt;
				}

				> .large {
					font-size: 30pt;
					line-height: 25pt;
					padding-left: 1.2pt;
				}

				> .justified {
					margin-top: 0;
					font-size: 11pt;
					line-height: 11pt;
					padding-right: 130px;
				}
			}
		}
		height: auto;
	}

	> .background {
		width: 100%;
		height: 100%;
		position: absolute;
	}

	.foreground {
		background-color: black;
		width: 100%;
		height: 100%;
		position: absolute;

		> .title {
			display: inline-block;
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
				font-weight: bolder;
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
				line-height: 17.5pt;
				font-weight: bold;
				text-align: justify;
				text-justify: distribute;
				margin-top: 5px;
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

.main {
	width: 100%;
	min-height: 200vh;
	transition-duration: 0.5s;

	&.padding-full {
		padding-top: 100vh;
	}

	&.padding-small {
		padding-top: $small-header-size;
		min-height: calc(100vh + #{$small-header-size});
	}

	*:first-child {
		margin-top: $small-header-size;
	}
}

</style>