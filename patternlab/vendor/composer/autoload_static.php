<?php

// autoload_static.php @generated by Composer

namespace Composer\Autoload;

<<<<<<< HEAD
class ComposerStaticInitbac8f5c4deb440fde28f86cb3f9a1478
=======
class ComposerStaticInitc442e26381269d1ca67fabc0636b7bbe
>>>>>>> meet-the-team
{
    public static $prefixLengthsPsr4 = array (
        'S' => 
        array (
            'Symfony\\Component\\Yaml\\' => 23,
            'Symfony\\Component\\Process\\' => 26,
            'Symfony\\Component\\Finder\\' => 25,
            'Symfony\\Component\\EventDispatcher\\' => 34,
            'Seld\\JsonLint\\' => 14,
        ),
    );

    public static $prefixDirsPsr4 = array (
        'Symfony\\Component\\Yaml\\' => 
        array (
            0 => __DIR__ . '/..' . '/symfony/yaml',
        ),
        'Symfony\\Component\\Process\\' => 
        array (
            0 => __DIR__ . '/..' . '/symfony/process',
        ),
        'Symfony\\Component\\Finder\\' => 
        array (
            0 => __DIR__ . '/..' . '/symfony/finder',
        ),
        'Symfony\\Component\\EventDispatcher\\' => 
        array (
            0 => __DIR__ . '/..' . '/symfony/event-dispatcher',
        ),
        'Seld\\JsonLint\\' => 
        array (
            0 => __DIR__ . '/..' . '/seld/jsonlint/src/Seld/JsonLint',
        ),
    );

    public static $prefixesPsr0 = array (
        'T' => 
        array (
            'Twig_' => 
            array (
                0 => __DIR__ . '/..' . '/twig/twig/lib',
            ),
        ),
        'S' => 
        array (
            'Symfony\\Component\\Filesystem\\' => 
            array (
                0 => __DIR__ . '/..' . '/symfony/filesystem',
            ),
        ),
        'P' => 
        array (
            'Pimple' => 
            array (
                0 => __DIR__ . '/..' . '/pimple/pimple/lib',
            ),
            'PatternLab\\PatternEngine\\Twig' => 
            array (
                0 => __DIR__ . '/..' . '/pattern-lab/patternengine-twig/src',
            ),
            'PatternLab' => 
            array (
                0 => __DIR__ . '/../..' . '/core/src',
                1 => __DIR__ . '/..' . '/pattern-lab/core/src',
            ),
        ),
        'M' => 
        array (
            'Michelf' => 
            array (
                0 => __DIR__ . '/..' . '/michelf/php-markdown',
            ),
        ),
        'G' => 
        array (
            'Guzzle\\Tests' => 
            array (
                0 => __DIR__ . '/..' . '/guzzle/guzzle/tests',
            ),
            'Guzzle' => 
            array (
                0 => __DIR__ . '/..' . '/guzzle/guzzle/src',
            ),
        ),
        'D' => 
        array (
            'Doctrine\\Common\\Collections\\' => 
            array (
                0 => __DIR__ . '/..' . '/doctrine/collections/lib',
            ),
        ),
        'C' => 
        array (
            'Colors' => 
            array (
                0 => __DIR__ . '/..' . '/kevinlebrun/colors.php/src',
            ),
        ),
        'A' => 
        array (
            'Alchemy' => 
            array (
                0 => __DIR__ . '/..' . '/alchemy/zippy/src',
            ),
        ),
    );

    public static function getInitializer(ClassLoader $loader)
    {
        return \Closure::bind(function () use ($loader) {
<<<<<<< HEAD
            $loader->prefixLengthsPsr4 = ComposerStaticInitbac8f5c4deb440fde28f86cb3f9a1478::$prefixLengthsPsr4;
            $loader->prefixDirsPsr4 = ComposerStaticInitbac8f5c4deb440fde28f86cb3f9a1478::$prefixDirsPsr4;
            $loader->prefixesPsr0 = ComposerStaticInitbac8f5c4deb440fde28f86cb3f9a1478::$prefixesPsr0;
=======
            $loader->prefixLengthsPsr4 = ComposerStaticInitc442e26381269d1ca67fabc0636b7bbe::$prefixLengthsPsr4;
            $loader->prefixDirsPsr4 = ComposerStaticInitc442e26381269d1ca67fabc0636b7bbe::$prefixDirsPsr4;
            $loader->prefixesPsr0 = ComposerStaticInitc442e26381269d1ca67fabc0636b7bbe::$prefixesPsr0;
>>>>>>> meet-the-team

        }, null, ClassLoader::class);
    }
}
