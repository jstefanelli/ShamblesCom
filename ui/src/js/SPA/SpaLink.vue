<template>
	<a :href="target" v-on:click="navigate">
		<slot />
	</a>
</template>

<script lang="ts" >
import { Vue, Component, Prop } from 'vue-property-decorator'
import SPA, { HttpMethod } from '@/SPA/spa';

@Component
export default class extends Vue {
	@Prop({default: "/"}) public target: string;
	@Prop({default: "GET"}) public method: HttpMethod;
	@Prop({default: null}) public reqData: any;

	navigate(ev: Event) {
		ev.preventDefault();
		ev.stopPropagation();
		SPA.navigate(this.target, this.method, this.reqData).then((page) => page.render()).catch((error) => console.error("[SPA Link] Error: ", error));
	}
}

</script>