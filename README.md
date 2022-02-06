# TyGoTech Code Quality Tool

![GitHub](https://img.shields.io/github/license/TyGoTech/lightweight.script.manager-tool-dotnet) ![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TyGoTech/lightweight.script.manager-tool-dotnet/publish)
![GitHub tag (latest by date)](https://img.shields.io/github/v/tag/TyGoTech/lightweight.script.manager-tool-dotnet) ![Nuget](https://img.shields.io/nuget/v/TyGoTech.Tool.LightweightScriptManager)

A dotnet tool for sharing code quality config files between repositories.

## Is This Useful?

Yes and No. If you want a fully packaged solution that just solves this problem then you are out of luck. However, if the problem of sharing boilerplate config files between your repositories is something that you are interested in then you should read on.

## Motivation

Although there have been many great developments in the `dotnet` ecosystem over the past few years, with many of these in code quality, one thing that still remains unsolved is how to easily share the boilerplate config files that this quality infrastructure depends upon between repositories.

Most seasoned teams and solo devs have standard `.editorconfig` files and `Directory.Build.props` in standard locations that they replicate between projects. But what has always bothered me is how to keep them in sync, because invariably, over time, optimizations and improvements are found and only the most rigorous devs would ever go to the trouble of replicating them back.

The upshot of this is that although each individual repo has an enforced coding standard, that standard may not be uniform across every repo *and if you are a quality pedant that is annoying*.

With the current trend towards microservice architectures (expect to see the return of the super-monolith by the end of the decade) this is not just an academic problem to ponder over buttered scones in the conservatory on a lazy Sunday morning, but is a real issue that could do with a decent solution.

There is currently an [open issue](https://github.com/dotnet/roslyn/issues/19028) with the dotnet team to address this, but it's been open since 2017, so while I have no doubt that it will get resolved one day, until that day comes some sort of Heath Robinson hack is needed to fudge this.

While I was looking for someone else's solution to this problem I came across [this post](https://devzone.channeladam.com/notebooks/languages/dotnet/editorconfig-distribution/), however I have a policy of not reading long posts on a white background that I can't understand in the first minute, so it could be that this completely nails the problem, but I thought it would be easier to roll my own than put my eyes through that kind of effort.

## Strategy

The standard config files that my organization uses are stored [here](https://github.com/TyGoTech/lightweight.script.manager-tool-dotnet/tree/main/resources) in this repo. Also stored in this repo, and published on [nuget](https://www.nuget.org/packages/TyGoTech.Tool.LightweightScriptManager/), is a `dotnet` tool that pulls down the latest version of these files and writes them to standard locations in a repo.

Because the standard config files get checked in to each repo there are no dependencies on the tool, but the tool provides a simple mechanism for updating the files uniformly with minimal hassle.

This also makes it easier to setup a new repo without needing to find the best candidate repo to crib from.

That's it. That's really all there is to it, it isn't very clever.

So this doesn't solve the problem of guaranteeing that the config files in each repo are at the latest version, but it does make it a lot easier to refresh the config files to the current standard and as far as I am concerned that is some sort of progress.

## Getting Started

Because your config files and repo structure are going to be different from ours and if this sounds like a better approach to maintaining standard config than your current process (probably none), then [fork this on github](https://github.com/TyGoTech/lightweight.script.manager-tool-dotnet/fork), tweak the code and publish your own tool on [nuget](https://www.nuget.org/). If you look at the code and see some good ways to generalize it then you are more than welcome to submit a PR. FYI it was written using VS Code, but it will probably also work in Visual Studio.

This approach works for any language, so you could use it for node sites, or if you are a real sadist maybe even Laravel, but if you are using Laravel then you probably have much bigger problems to worry about than uniformity.

## Usage

Install the tool globally

```[bash]
dotnet tool install --global TyGoTech.Tool.LightweightScriptManager
```

Navigate to the root folder of your repo and initialize it

```[bash]
tygotech.codequality init
```

This will generate a settings file called `codequalityrc.json` in the root of the repo that looks something like this

```[json]
{
  "sourceFolder": "src/",
  "testFolder": "src/test/",
  "resourcesUri": "https://raw.githubusercontent.com/TyGoTech/lightweight.script.manager-tool-dotnet/main/resources/",
  "noTest": false
}
```

Now fetch the latest config files and put them in the standard locations

```[bash]
tygotech.codequality fetch
```

The `fetch` command is used for both the first fetch as well as subsequent update fetches.

One thing to note is that in the source directory three `*.Build.props` files are created

- `Directory.Build.props` - picked up by the build.
- `Shared.Build.props` - contains the shared build props and is referenced by the `Directory.Build.props`.
- `Override.Build.props` - when initialized this is empty, however it can be modified and is be overwritten on subsequent fetches. This can be used for repo specific settings like

    ```[xml]
    <Project>
        <PropertyGroup>
            <Company>TyGoTech</Company>
            <Product>Code Quality Tool</Product>
            <Copyright>© TyGoTech 2022</Copyright>
        </PropertyGroup>
    </Project>
    ```
