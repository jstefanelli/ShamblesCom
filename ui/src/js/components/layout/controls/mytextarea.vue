<template>
	<textarea class="bg-black border-2 border-blac border-opacity-50 p-3 text-xl text-white rounded-md hover:border-white focus:border-opacity-100 focus:border-white active:border-opacity-100 active:border-white"
	 :value="value" @input="$emit('input', $event.target.value)" :disabled="disabled">

	</textarea>
</template>

<script lang="ts">

import { Vue, Component, Prop } from 'vue-property-decorator';

@Component({
    components: {

    }
})
export default class extends Vue {
    @Prop({default: undefined}) public value: any;
    @Prop({default: false}) public disabled: boolean;
}

</script>