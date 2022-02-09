# TyGoTech Lightweight Script Manager

![GitHub](https://img.shields.io/github/license/TyGoTech/tool-lightweight.script.manager) ![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TyGoTech/tool-lightweight.script.manager/publish)
![GitHub tag (latest by date)](https://img.shields.io/github/v/tag/TyGoTech/tool-lightweight.script.manager) ![Nuget](https://img.shields.io/nuget/v/TyGoTech.Tool.LightweightScriptManager)

*A tool for sharing files between repositories.*

## Motivation

Although there have been many great developments in the `dotnet` ecosystem over the past few years, one thing that still remains unsolved is how to easily share boilerplate config files between repositories. [Nuget](https://www.nuget.org/) works well for individual projects, but not for arbitrary folders.

Most seasoned teams and solo devs have standard `.editorconfig` files and `Directory.Build.props` in standard locations that they replicate between repos. But what has always bothered me is how to keep them in sync, because invariably, over time, optimizations and improvements are found and only the most rigorous devs go to the trouble of replicating them back.

The upshot of this is that although each individual repo has an enforced coding standard, this standard may not be uniform across every repo and *if you are a quality pedant this is annoying*.

With the current trend towards microservice architectures (expect to see the return of the super-monolith by the end of the decade) this is not just an academic problem to ponder over buttered scones in the conservatory on a lazy Sunday morning, but is a real issue that could do with a decent solution.

There is currently an [open issue](https://github.com/dotnet/roslyn/issues/19028) with the dotnet team to address this, but it's been open since 2017, so while I have no doubt that it will get resolved one day, until that day comes some sort of Heath Robinson hack is needed.

While I was looking for someone else's solution to this problem I came across [this post](https://devzone.channeladam.com/notebooks/languages/dotnet/editorconfig-distribution/), however I have a policy of not reading long posts on a white background that I can't understand in the first minute, so it could be that this completely nails the problem, but I thought it would be easier to roll my own than endure unnecessary eye strain.

## Getting Started

Install the dotnet tool.
  
```[bash]
dotnet tool install --global TyGoTech.Tool.LightweightScriptManager
```

### Create an LSM package

- Create a new GitHub repo. This is where you will store and maintain the files that you intend to share between you repositories. Currently this needs to be a public repo, but feel free to submit a PR to support private repos, [see here](https://gist.github.com/MaximRouiller/74ae40aa994579393f52747e78f26441).
- Clone the repo locally.

    ```[bash]
    git clone git@github.com:[user]/[repo].git
    ```

- Open the repo in your favorite non-super-bloatware IDE (think VS Code not Visual Studio).
- Add the files that you want to share between your repos using the directory structure that you want to deploy them into. See [`resources/dotnet`](https://github.com/TyGoTech/tool-lightweight.script.manager/tree/main/resources/dotnet) for an example.
- Open a terminal in the root of your local repo.
- Build a runtime config file.

    ```[bash]
    tygotech.lsm build
    ```

    This will generate a file called `lsmrc.json` in the current directory. See [`lsmrc.json`](https://github.com/TyGoTech/tool-lightweight.script.manager/blob/main/resources/dotnet/lsmrc.json) for an example.
- Commit and push.

    ```[bash]
    git commit -am "feat: add lsm config files" && git push
    ```

### Use the LSM package

- Open a terminal in the root of a local copy of the repo where you want to share the files.
- Initialize the repo, passing in the URI of the package repository.

    ```[bash]
    tygotech.lsm init --package-uri https://github.com/[user]/[repo]
    ```

    This pulls down the `lsmrc.json` file from the package repo.
- Fetch the files from the package repo into the package directory structure.

    ```[bash]
    tygotech.lsm fetch
    ```

Rerunning `fetch` command will fetch the latest versions of the files in the package repo.

## `lsmrc.json` settings

The generated config file has a schema associated with it, so it provides autocomplete options and enough documentation to hopefully be self-describing. However, one useful thing to note is the `preserve` property that can be set on each resource map. When this is `true` then subsequent fetches will *not* overwrite and existing file.
