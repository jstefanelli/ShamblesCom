<template>
		<div class="flex overflow-hidden items-center m-[5px]">
			<div class=" w-3 self-stretch" :style="'background-color: #' + mainColor" ></div>
			<div class=" w-3 self-stretch mr-3" :style="'background-color: #' + secondaryColor" ></div>
			<div class="font-l">
				<slot ></slot>
			</div>
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
	@Prop({default: "000"}) readonly mainColor: string;
	@Prop({default: "FFF"}) readonly secondaryColor: string;
}

</script>