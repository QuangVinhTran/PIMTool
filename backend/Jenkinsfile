pipeline{
    agent any
    stages{
        stage("Build"){
            steps{
                sh "cp D:/.Programming/C#/Projects/pimtool-monolithic/.deployment ./"
                sh "sh ./.deployment/deployment.sh"
                sh "docker build -f ./PIMTool/Dockerfile -t ${DOCKER_REGISTRY}/pimtool-monolithic ."
            }
        }
        stage("Deliver"){
            steps{
                withDockerRegistry(credentialsId: 'duykasama-docker-hub', url: "https://index.docker.io/v1/") {
                    sh "docker push ${DOCKER_REGISTRY}/pimtool-monolithic"
                }
            }
        }
        stage("Deploy"){
            steps{
                sh "docker run --name pimtool-monolithic -p 10000:80 --network pimtool ${DOCKER_REGISTRY}/pimtool-monolithic"
            }
        }
    }
    post{
        always{
            cleanWs()
        }
    }
}