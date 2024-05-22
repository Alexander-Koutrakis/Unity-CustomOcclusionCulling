# Custom Occlusion Culling System for Unity

## Overview

This Unity project implements a custom occlusion culling system to efficiently manage the visibility of objects in a 3D scene. The system uses a custom shader and asynchronous GPU readback to determine which objects (cells) are visible to the camera, enhancing performance by rendering only the necessary elements.This method of occlusion is lightweight on the CPU compared to Unity's built-in occlusion system, as all checks are performed on the GPU.This project is based on [przemyslawzaworski's Unity-GPU-Based-Occlusion-Culling](https://github.com/przemyslawzaworski/Unity-GPU-Based-Occlusion-Culling).This tool is particularly useful for occluding large groups of renderers. The primary use case and reason for development was hiding interiors in open-world enviroments.

Occlusion Off:

![WithoutOcclusion-reduced](https://github.com/Alexander-Koutrakis/Custom-Occlusion-Culling/assets/61294700/c8d38cd6-65e0-4730-9c86-a0953b766689)

Draw calls (30-100)

Occlusion On:

![WithOcclusion-reduced](https://github.com/Alexander-Koutrakis/Custom-Occlusion-Culling/assets/61294700/8ea2cb03-5e65-493c-b288-ab1110bfbb4f)

Draw calls (9-18)

## How It Works

1. **Creating Cells**:
   - Cells are created based on the `Cell` class, representing individual objects in the scene.
   - Cells are passed to a buffer and sent to the shader for processing.

2. **Collision Check**:
   - The vertex shader checks if the camera is inside the cell to set the visibility.

3. **Visibility Check**:
   - The pixel shader uses the attribute `[earlydepthstencil]` to force depth-stencil testing before the shader executes, checking if each cell is visible.

4. **Renderer Management**:
   - The `OccludeeArea` class checks if any of its cells are visible and enables or disables the renderers within its area accordingly.

## Components

### Cell Class

Defines individual objects in the scene, including their visibility and collision-only status. Provides functionality to draw wireframe cubes for debugging.

### CellGenerator Class

Generates vertices and triangle indices for the cells, transforming them based on their position, rotation, and scale.

### OcclusionManager Class

Manages the occlusion culling process, initializing GPU buffers, sending data for occlusion checks, and updating cell visibility based on GPU results.

### OccludeeArea Class

Manages visibility of groups of cells, showing or hiding renderers based on the visibility of contained cells.

### Occlusion Shader

A custom shader used for occlusion culling. It checks the visibility and collision of the Cells with the camera.

## Debugging

- Enable the debug mode by setting the `isDebugOn` property of the `OcclusionManager` to `true`.
- When in debug mode, the occlusion shader will render objects with a semi-transparent blue color to visualize the occlusion process.

## Additional Notes

- GPU async communication requires at least one frame, so the results are not instant. The alternative would make the CPU wait for too long, which is avoided to maintain performance.
- Editor camera is also considered in the shader visibility checks.



