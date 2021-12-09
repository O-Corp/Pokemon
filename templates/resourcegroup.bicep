targetScope = 'subscription'

param resourceGroupName string

module rgModule 'module.bicep' = {
  name: 'rgModule'
  scope: resourceGroup(resourceGroupName)
}
