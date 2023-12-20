# App  
* Initial
```bash
kubectl create namespace group-1-bojdev
```
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


# Collector

## Agent

cd to deployment/collector

```bash
docker run --name agentcloud -d -e AGENT_MODE=flow -v $(pwd)/config.river:/etc/agent/config.river -p 9999:9999 -p 12345:12345 -p 4318:4318 -p 4317:4317 grafana/agent run --server.http.listen-addr=0.0.0.0:12345 /etc/agent/config.river
```

```PS
docker run --name agentcloud -d -e AGENT_MODE=flow -v ${pwd}/config.river:/etc/agent/config.river -p 9999:9999 -p 12345:12345 -p 4318:4318 -p 4317:4317 grafana/agent run --server.http.listen-addr=0.0.0.0:12345 /etc/agent/config.river
```

K8S Collector Deployment

```bash
cd deployment/collector
kubectl create namespace group-1-obs
kubectl create secret generic bojappobs-secrets --from-file=config.river --dry-run=client -o yaml > bojappobs-secrets.yaml -n group-1-obs

kubectl create deployment bojappobs --image=grafana/agent -n group-1-obs --dry-run=client -o yaml > bojappobs.yaml
kubectl expose deployment bojappobs --name=bojappobs-svc --port=4317 --target-port=4317 -n group-1-obs -o yaml --dry-run=client -o yaml > bojappobs-svc.yaml
```

kubectl port-forward deploy/bojappobs 12345:12345 -n group-1-obs

kubectl port-forward deploy/bojappobs 4318:4318 -n group-1-obs
kubectl port-forward deploy/bojappobs 4317:4317 -n group-1-obs


kubectl run --rm -it --tty pingkungcurl1 -n group-1-bojdev --image=curlimages/curl --restart=Never -- bojappobs-svc.group-1-obs:12345

ref: https://github.com/grafana/agent/blob/main/production/kubernetes/agent-loki.
ref: https://grafana.com/docs/agent/latest/flow/setup/install/docker/