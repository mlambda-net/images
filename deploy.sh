deploy() {
  case $1 in
  dev) deploy_dev ;;
  prod) deploy_prod ;;
  esac
}

deploy_dev() {
  TAG="$(git describe --tags --dirty)"
  export ENV="dev"
  export VERSION="${TAG:-1.0.0}"
  echo "deploying dev version " $VERSION
  skaffold dev -p QA
}

deploy_prod() {
  TAG="$(git describe --tags --abbrev=0)"
  export ENV="prod"
  export VERSION="${TAG:-1.0.0}"
  echo "deploying prod version " $VERSION
  skaffold run -p PROD
}

echo "$DOCKERPASS" | docker login docker.pkg.github.com -u RoyGI --password-stdin
deploy "$1"
