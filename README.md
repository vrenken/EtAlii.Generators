# EtAlii.Generators

## Introduction
Code generation is fun and when done right extremely helpful. This project aims to use a combination of [ANTLR](https://github.com/antlr/antlr4) and [Roslyn source generators](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md) to simplify usage of some of the more mainstream frameworks/libraries.
The first attempt is to use PlantUML to visually design [Stateless](https://github.com/dotnet-state-machine/stateless) state machines.

## Project status

[![Build](https://github.com/vrenken/EtAlii.Generators/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/vrenken/EtAlii.Generators/actions/workflows/build.yml)
[![Analysis](https://github.com/vrenken/EtAlii.Generators/actions/workflows/analysis.yml/badge.svg)](https://github.com/vrenken/EtAlii.Generators/actions/workflows/analysis.yml)
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![GitHub](https://img.shields.io/github/license/vrenken/EtAlii.Generators)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=bugs)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=code_smells)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=coverage)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=ncloc)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=sqale_index)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=security_rating)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=EtAlii.Generators&metric=alert_status)](https://sonarcloud.io/dashboard?id=EtAlii.Generators)


|Generation | Documentation | Version | Downloads |
|:---|:---|:---|:---|
| PlantUML diagram to Stateless C# code | [Link](EtAlii.Generators.Stateless.Tests/README.md) | [![Nuget](https://img.shields.io/nuget/v/EtAlii.Generators.Stateless)](https://www.nuget.org/packages/EtAlii.Generators.Stateless) | [![Nuget](https://img.shields.io/nuget/dt/EtAlii.Generators.Stateless)](https://www.nuget.org/packages/EtAlii.Generators.Stateless) |
| Behavior tree to C# code | Tbd. | [![Nuget](https://img.shields.io/nuget/v/EtAlii.Generators.Behavior)](https://www.nuget.org/packages/EtAlii.Generators.Behavior) | [![Nuget](https://img.shields.io/nuget/dt/EtAlii.Generators.Behavior)](https://www.nuget.org/packages/EtAlii.Generators.Behavior) |
| ML pipeline diagram to C# code | Tbd. | [![Nuget](https://img.shields.io/nuget/v/EtAlii.Generators.MlPipeline)](https://www.nuget.org/packages/EtAlii.Generators.MlPipeline) | [![Nuget](https://img.shields.io/nuget/dt/EtAlii.Generators.MlPipeline)](https://www.nuget.org/packages/EtAlii.Generators.MlPipeline) |
