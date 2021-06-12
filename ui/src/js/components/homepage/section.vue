<template>
	<div class="card no-hover">
		<slot />
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