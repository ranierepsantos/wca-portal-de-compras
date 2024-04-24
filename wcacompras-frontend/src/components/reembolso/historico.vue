<template>
    <v-card>
        <v-breadcrumbs>
            <div class="text-h5 text-primary">HISTÓRICO</div>
            <v-spacer></v-spacer>
            <v-btn
            icon="$close"
            density="comfortable"
            variant="plain"
            color="primary"
            @click="$emit('closeClick')"
            >
            </v-btn>
        </v-breadcrumbs>
        
        <v-card-text>
          <v-row>
              <v-col class="text-left">
                  <v-timeline align="start" side="end">
                      <v-timeline-item dot-color="grey" size="small" v-for="item in eventos">
                          <div class="d-flex">
                              <div>
                                  <span class="text-body-1" v-html="moment(item.dataHora).add(timeDifference,'hour').format('DD/MM/YYYY HH:mm:ss') + ' - ' + item.evento"></span>
                              </div>
                          </div>
                      </v-timeline-item>
                  </v-timeline>
              </v-col>
          </v-row>
        </v-card-text>
      </v-card>
</template>

<script setup>
    import moment from 'moment';
    defineProps({
        eventos: {
            type: Array,
            default: function() {return []}
        }, 
    })
    const timeDifference = process.env.VUE_APP_TIMEDIFFERENCE ?? -3
    
</script>