<template>
	<div :class="'bg-opacity-50 bg-bg-second rounded-tr-xl rounded-bl-xl ' + (!disabled ? 'border-t-white border-t-4 border-r-white border-r-4 border-b-0 border-l-0 text-opacity-100' : ' text-opacity-50 border-t-transparent border-t-4 border-r-transparent border-r-4 border-b-0 border-l-0')" @click="$emit('click')">
		<slot ></slot>
	</div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator';

@Component({
	components: {
	}
})
export default class extends Vue {
	@Prop({default: false}) public disabled: boolean;
}
</script>