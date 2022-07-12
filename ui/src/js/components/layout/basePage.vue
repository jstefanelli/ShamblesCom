<template>
	<div class="w-full box-border min-h-screen text-white bg-bg">
		<header class="fixed w-screen bg-red-titlebar h-28 flex flex-row items-center font-semibold text-2xl font-haas z-50">
			<div class="flex-grow flex items-center">
				<span class="flex-grow" ></span>
				<spa-link target="/drivers" class=" italic">
					<h3>Drivers</h3>
				</spa-link>
				<span class="flex-grow" ></span>
				<spa-link target="/teams" class=" italic">
					<h3>Teams</h3>
				</spa-link>
				<span class="flex-grow" ></span>
			</div>
			<div class="logo">
				<spa-link class="block w-60 h-24" target="/homepage" >
					<img :src="logo" class="w-full h-full object-center object-contain">
				</spa-link>
			</div>
			<div class="buttons flex-grow flex items-center">
				<span class="flex-grow" ></span>
				<spa-link target="/schedule" class="italic">
					<h3>Schedule</h3>
				</spa-link>
				<span class="flex-grow" ></span>
				<spa-link target="/highlights" class="italic">
					<h3>Highlights</h3>
				</spa-link>
				<span class="flex-grow" ></span>
			</div>
		</header>
		<main class="pt-28 min-h-screen flex items-stretch justify-center">
			
			<div class=" max-w-[1320px] flex-grow">
				<slot ></slot>
			</div>
		</main>
	</div>
</template>

<script lang="ts">
import SpaLink from '@/SPA/SpaLink.vue';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import Logo from '£/images/logo.png';

@Component({
	components: {
		SpaLink
	}
})
export default class extends Vue {
	public logo = Logo;
}

</script>