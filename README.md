# tcc-group-1

* Overview Slide: https://docs.google.com/presentation/d/134SXFtcSt6Epts3yitRrEu9-DqsZGS8AZH9fPW11PKE/edit#slide=id.p

## Getting Start

* [Front End](/front/BankOfJumpbox/README.md)
* [Back End](/back/bojpawnapi/readme.md)
* Deployment
  - CMD [app/collector](/deployment/readme.md)
  - CMD [database](/deployment/db/dev_insecure/readme.md)
  - GitOps
    * Argo Config : https://github.com/the-cloud-camp/tcc-k8s-app ตอนนี้มีแยก [App Dev](https://github.com/the-cloud-camp/tcc-k8s-app/blob/main/dev/app-group-1.yaml) / [App Prod](https://github.com/the-cloud-camp/tcc-k8s-app/blob/main/prod/app-group-1.yaml) / ส่วน DB (ns group-1-db) กับ grafrana agent (ns group-1-obs) ยังไม่ได้ทำ
    * Dev : https://github.com/the-cloud-camp/tcc-k8s-config-nonprd/tree/group-1
    * Prod : https://github.com/the-cloud-camp/tcc-k8s-config/tree/group-1
* Dev Swagger: https://tcc-01.th1.proen.cloud/bojpawndevback/swagger/index.html
* Dev EndPoint: https://tcc-01.th1.proen.cloud/bojpawndev
* Prod Frontend EndPoint: https://tcc-01.th1.proen.cloud/bojpawn
* Prod Backend EndPoint: https://tcc-01.th1.proen.cloud/bojpawnback/health/ready

