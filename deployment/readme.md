# App 
* frontends

``` 
kubectl create configmap bojpawnfrontconfig --from-literal=ENDPOINTBACKEND=bojpawnapi-svc.group-1-bojdev -n group-1-bojdev --dry-run=client -o yaml > dev_bojpawnfrontconfig.yaml

kubectl create deployment bojpawnfront --image=pingkunga/bojpawnfront:0.0.1 -n group-1-bojdev --dry-run=client -o yaml > dev_bojpawnfront.yaml

kubectl expose deployment bojpawnfront --name=bojpawnfront-svc --port=80 --target-port=3000 -n group-1-bojdev -o yaml --dry-run=client -o yaml > dev_bojpawnfront-svc.yaml

kubectl autoscale deployment bojpawnfront --cpu-percent=70 --min=1 --max=3 -n group-1-bojdev --dry-run=client -oyaml > dev_bojpawnfront_hpa.yaml

#Check Health in K8S
kubectl run --rm -it --tty pingkungcurl1 -n group-1-bojdev --image=curlimages/curl --restart=Never -- bojpawnapi-svc.group-1-bojdev/health/startup

``` 

* backends

```
kubectl create configmap bojpawnapiconfig --from-file=../../back/bojpawnapi/appsettings.json --from-file=../../back/bojpawnapi/appsettings.Development.json -n group-1-bojdev --dry-run=client -o yaml > dev_bojpawnapiconfig.yaml

#kubectl create secret generic bojpawnapisecret --from-literal=secretmessage=mysecret -n group-1-bojdev --dry-run=client -o yaml > dev_bojpawnapisecret.yaml

kubectl create secret generic bojpawnapisecretdev --from-file="ConnectionStrings_BojPawnDbConnection"=env_connstring.txt --from-file="JWTKey_Secret"=env_secret.txt --from-file="ObservabilityOptions_CollectorUrl"=env_obs_url.txt -n group-1-bojdev --dry-run=client -o yaml > dev_bojpawnapisecret.yaml

kubectl create deployment bojpawnapi --image=pingkunga/bojpawnapi:0.0.3 -n group-1-bojdev --dry-run=client -o yaml > dev_bojpawnapi.yaml

kubectl expose deployment bojpawnapi --name=bojpawnapi-svc --port=80 --target-port=8090 -n group-1-bojdev -o yaml --dry-run=client -o yaml > dev_bojpawnapi-svc.yaml

kubectl autoscale deployment bojpawnapi --cpu-percent=70 --min=1 --max=3 -n group-1-bojdev --dry-run=client -oyaml > dev_bojpawnapi_hpa.yaml


#Check Health in K8S
kubectl run --rm -it --tty pingkungcurl1 -n group-1-bojdev --image=curlimages/curl --restart=Never -- bojpawnapi-svc.group-1-bojdev/health/startup

มันจะขึ้น Healthy / unhealthy
```

Apply Change

kubectl get deployment,pod,svc,sts -n group-1-bojdev 

# Export BackEnd for Dev

kubectl port-forward deploy/bojpawnapi 8090:8090 -n group-1-bojdev

http://localhost:8090/swagger/index.html

# Test Connect DB

kubectl run --rm -it pinggrpcurl -n group-1-bojdev --image=fullstorydev/grpcurl -- -plaintext cockroachdb-public.group-1-bojdev:26257 list

#ต้องเข้าไปดู Log เอง มันไม่ยอม List Method ออกมา

# Test Connect Back Service

kubectl run --rm -it --tty pingkungcurl1 -n group-1-bojdev --image=curlimages/curl --restart=Never -- bojpawnapi-svc.group-1-bojdev/api/Customer
