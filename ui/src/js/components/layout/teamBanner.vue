<template>
		<div :class="'team-banner'">
			<div class="color-main" :style="'background-color: #' + mainColor" />
			<div class="color-secondary" :style="'background-color: #' + secondaryColor" />
			<div class="font-l2">
				<slot />
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

<style lang="scss">
@import "#/colors";

.team-banner {
	display: flex;
	overflow: hidden;
	margin: 5px;

	> .color-main {
		width: 10px;
	}

	> .color-secondary {
		width: 10px;
		margin-right: 10px;
	}
}
</style>