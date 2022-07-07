<template>
    <select :value="value" @input="$emit('input', $event.target.value)" @change="$emit('change')" :class="{
        'bg-black text-white border-opacity-50 border-black border-2 rounded-md p-3 appearance-none font-sans': true
    }">
        <slot></slot>
    </select>
</template>

<script lang="ts">

import { Vue, Component, Prop } from 'vue-property-decorator';

@Component({
    components: {

    }
})
export default class extends Vue {
    @Prop({default: undefined}) public value: any;
}

</script>