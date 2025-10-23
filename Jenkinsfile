pipeline {
      agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:7.0'
        }
    }

    stages {
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
			// Archive MSTest results using xUnit plugin
			xunit(
				tools: [
					MSTest(deleteOutputFiles: true, failIfNotNew: false, pattern: '**/TestResults/*.trx', skipNoTestFiles: false, stopProcessingIfError: true)
				]
				)
			}
		}
}
