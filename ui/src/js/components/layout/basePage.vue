<template>
	<div class="fill-w">
		<header class="titlebar flex align-center font-m">
			<div class="buttons flex-grow flex align-center">
				<span class="flex-grow" />
				<spa-link target="/drivers" class="bold italics">
					Drivers
				</spa-link>
				<span class="flex-grow" />
				<spa-link target="/standings" class="bold italics">
					Standings
				</spa-link>
				<span class="flex-grow" />
			</div>
			<div class="logo">
				<spa-link target="/homepage" />
			</div>
			<div class="buttons flex-grow flex align-center">
				<span class="flex-grow" />
				<spa-link target="/schedule" class="bold italics">
					Schedule
				</spa-link>
				<span class="flex-grow" />
				<spa-link target="/highlights" class="bold italics">
					Highlights
				</spa-link>
				<span class="flex-grow" />
			</div>
		</header>
		<main>
			<slot />
		</main>
	</div>
</template>

<script lang="ts">
import SpaLink from '@/SPA/SpaLink.vue';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';

@Component({
	components: {
		SpaLink
	}
})
export default class extends Vue {

}

</script>

<style lang="scss">
@import "#/colors";

header.titlebar {
	position: fixed;
	width: 100%;
	height: 100px;
	background-color: $titlebar-color;
}

.logo {
	> a {
		display: block;
		background-image: url("£/images/logo.png");
		background-size: contain;
		background-repeat: no-repeat;
		background-position: center center;
		width: 236px;
		height: 74px;
	}
}

main {
	padding-top: 100px;
	min-height: 100vh;
}
</style>