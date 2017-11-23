Quick Start を元にやってみる。

* [Quick Start \- Spark 2\.2\.0 Documentation](https://spark.apache.org/docs/latest/quick-start.html)

最新をダウンロード。

```
# wget http://ftp.jaist.ac.jp/pub/apache/spark/spark-2.2.0/spark-2.2.0-bin-hadoop2.7.tgz
# tar -zxvf spark-2.2.0-bin-hadoop2.7.tgz
```

実行してみる。

```
# ./bin/run-example SparkPi 10
JAVA_HOME is not set
```

Javaをインストールしないとダメ。
```
# yum install -y java-1.8.0-openjdk
```

とりあえず動いた。

```
# ./bin/run-example SparkPi 10
Using Spark's default log4j profile: org/apache/spark/log4j-defaults.properties
17/11/23 12:23:38 INFO SparkContext: Running Spark version 2.2.0
```

Spark Shell で確認してみる。

```
# ./bin/spark-shell
Spark session available as 'spark'.
Welcome to
      ____              __
     / __/__  ___ _____/ /__
    _\ \/ _ \/ _ `/ __/  '_/
   /___/ .__/\_,_/_/ /_/\_\   version 2.2.0
      /_/

Using Scala version 2.11.8 (OpenJDK 64-Bit Server VM, Java 1.8.0_151)
Type in expressions to have them evaluated.
Type :help for more information.

scala>
scala> val textFile = spark.read.textFile("README.md")
textFile: org.apache.spark.sql.Dataset[String] = [value: string]

scala> textFile.count()
res0: Long = 103
```

sbtのインストール。
* [sbt Reference Manual — Installing sbt on Linux](http://www.scala-sbt.org/release/docs/Installing-sbt-on-Linux.html#Red+Hat+Enterprise+Linux+and+other+RPM-based+distributions)

```
# curl https://bintray.com/sbt/rpm/rpm | sudo tee /etc/yum.repos.d/bintray-sbt-rpm.repo
# sudo yum install sbt
```

sbt叩いてみたがエラー。
```
# sbt
/usr/share/sbt/bin/sbt-launch-lib.bash: line 207: bc: command not found

The java installation you have is not up to date
requires at least version 1.6+, you have
version 1.8

Please go to http://www.java.com/getjava/ and download
a valid Java Runtime and install before running .
```

bcを入れる。
```
# yum install -y bc
```

サンプルコードを書いてみる。
```
# mkdir -p src/main/scala
# vi src/main/scala/SimpleApp.scala
```

```scala
import org.apache.spark.sql.SparkSession

object SimpleApp {
  def main(args: Array[String]) {
    val logFile = "YOUR_SPARK_HOME/README.md" // Should be some file on your system
    val spark = SparkSession.builder.appName("Simple Application").getOrCreate()
    val logData = spark.read.textFile(logFile).cache()
    val numAs = logData.filter(line => line.contains("a")).count()
    val numBs = logData.filter(line => line.contains("b")).count()
    println(s"Lines with a: $numAs, Lines with b: $numBs")
    spark.stop()
  }
}
```

```
# vi build.sbt
```
```
name := "Simple Project"

version := "1.0"

scalaVersion := "2.11.8"

libraryDependencies += "org.apache.spark" %% "spark-sql" % "2.2.0"
```

ビルド。時間がかかる。。。
```
# sbt package
```

実行。とりあえず動いた。

```
# ./bin/spark-submit --class "SimpleApp" --master local[4] sample/target/scala-2.11/simple-project_2.11-1.0.jar
```
