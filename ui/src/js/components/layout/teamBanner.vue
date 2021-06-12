<template>
		<div :class="'team-banner ' + teamClass() ">
			<div class="color-main" />
			<div class="color-secondary" />
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
	@Prop({default: 'renault'}) readonly team: string;

	teamClass() {
		return "team-" + this.team;
	}
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