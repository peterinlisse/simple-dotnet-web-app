pipeline {
      agent {
        docker {
             image 'dotnet-docker-agent:latest'
            args '-v /var/run/docker.sock:/var/run/docker.sock'
        }
    }

    stages {
	
		stage('Start SQL Container') {
            steps {
                // Run SQL container from basetestcontainer:v1 image
                sh '''
                    docker run -d --name sqltestcontainer \
                    -e "ACCEPT_EULA=Y" \
                    -e "SA_PASSWORD=Password1!" \
                    -p 1433:1433 \
                    basetestcontainer:v1
                '''
            }
        }
		
        stage('Checkout') {
		  steps {
			git branch: 'main',
				url: 'https://github.com/peterinlisse/jenkinstest.git',
				credentialsId: 'git1'
		  }
		}

        stage('Restore') {
            steps {
                // Restore NuGet packages
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                // Build the solution
                sh 'dotnet build LocalLibJenkinsTest1.sln --configuration Release'
            }
        }

        stage('Test') {
            steps {
                // Run MSTest tests
                sh 'dotnet test LocalLibJenkinsTest1.sln --logger "trx;LogFileName=TestResults.trx" --configuration Release'
            }
        }
	}
	
	post {
			always {
                    // Archive test results for Jenkins
                    mstest testResultsFile: '**/TestResults/*.trx'
                }
		}
}
