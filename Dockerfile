FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine

LABEL maintainer="Jasper Arildslund <jbjar@cbb.dk>"
LABEL repository="https://github.com/mvno/favro-updater"
LABEL homepage="https://github.com/mvno/favro-updater"

LABEL com.github.actions.name="Favro updater"
LABEL com.github.actions.description="A Github action that updates Favro cards"
LABEL com.github.actions.icon="sliders"
LABEL com.github.actions.color="purple"

RUN wget https://github.com/mvno/favro-updater/releases/download/v0.0.7/FavroUpdater.Console.0.0.7.zip \
&& unzip FavroUpdater.Console.0.0.7.zip -d . \
&& rm FavroUpdater.Console.0.0.7.zip

ENTRYPOINT [ "dotnet", "/FavroUpdater.Console.dll" ]
