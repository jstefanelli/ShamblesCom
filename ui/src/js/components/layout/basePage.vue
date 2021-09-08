<template>
	<div class="fill-w">
		<header class="titlebar flex align-center font-m font-haas">
			<div class="buttons flex-grow flex align-center">
				<span class="flex-grow" />
				<spa-link target="/drivers" class="italics plain">
					Drivers
				</spa-link>
				<span class="flex-grow" />
				<spa-link target="/standings" class="italics plain">
					Standings
				</spa-link>
				<span class="flex-grow" />
			</div>
			<div class="logo">
				<spa-link class="background-logo" target="/homepage" />
			</div>
			<div class="buttons flex-grow flex align-center">
				<span class="flex-grow" />
				<spa-link target="/schedule" class="italics plain">
					Schedule
				</spa-link>
				<span class="flex-grow" />
				<spa-link target="/highlights" class="italics plain">
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
		width: 236px;
		height: 74px;
	}
}

main {
	padding-top: 100px;
	min-height: 100vh;

	> .page-content {
		min-height: calc(100vh - 100px);
	}
}
</style>