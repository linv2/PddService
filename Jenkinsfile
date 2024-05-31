pipeline {
  agent {
    docker {
      registryUrl 'http://xxxxx.coding.net'
      image 'pddservice/docker/dotnetsdk:v3.1'
      registryCredentialsId env.DOCKER_REGISTRY_CREDENTIALS_ID
    }

  }
  stages {
    stage('检出') {
      steps {
        sh 'dotnet --version'
        checkout([$class: 'GitSCM', branches: [[name: env.GIT_BUILD_REF]], 
                                                                                                                                                                                                                                                                                                                  userRemoteConfigs: [[url: env.GIT_REPO_URL, credentialsId: env.CREDENTIALS_ID]]])
        echo '环境准备完毕'
      }
    }
    stage('编译') {
      steps {
        echo '开始还原包'
        sh 'dotnet restore src/ --configfile .nuget/NuGet.Config'
        echo '开始编译项目'
        sh 'dotnet publish --configuration  Release src/PddService.sln'
        echo '编译完成'
      }
    }
    stage('文件打包') {
      steps {
        sh 'tar zcvf release.tar.gz  -C src/PddService/bin/Release/ publish/'
        sh 'curl  -T release.tar.gz -u '+PROJECT_TOKEN_GK+':'+PROJECT_TOKEN+' "https://xxxxxx.coding.net/PddService/release/release.tar.gz?VERSION='+CI_BUILD_NUMBER+'"'
      }
    }
  }
}