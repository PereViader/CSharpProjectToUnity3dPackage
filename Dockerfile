# Set the base image as the .NET 5.0 SDK (this includes the runtime)
FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env

# Copy everything and publish the release (publish implicitly restores and builds)
COPY . ./
RUN dotnet publish ./CSharpProjectToUnity3dPackage/CSharpProjectToUnity3dPackage.csproj -c Release -o out --no-self-contained

# Label the container
LABEL maintainer="Pere Viader <pere.viader22@gmail.com>"
LABEL repository="https://github.com/PereViader/CSharpProjectToUnity3dPackage"
LABEL homepage="https://github.com/PereViader/CSharpProjectToUnity3dPackage"

# Label as GitHub action
LABEL com.github.actions.name="CSharpProjectToUnity3dPackage"
# Limit to 160 characters
LABEL com.github.actions.description="Convert a c# project into a unity package"
# See branding:
# https://docs.github.com/actions/creating-actions/metadata-syntax-for-github-actions#branding
LABEL com.github.actions.icon="activity"
LABEL com.github.actions.color="white"

# Relayer the .NET SDK, anew with the build output
FROM mcr.microsoft.com/dotnet/sdk:5.0
COPY --from=build-env /out .
ENTRYPOINT [ "dotnet", "/CSharpProjectToUnity3dPackage.dll" ]